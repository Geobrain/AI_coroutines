using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using MEC;
using Pixeye.Actors;
using Pixeye.Actors.Maths;
using Pixeye.Source;
using UnityEngine;
using Time = Pixeye.Actors.time;


public class ProcessorAI : Processor, ITick
{
    
	Group<ComponentAI> source;
	
	IEnumerator<float> AI_Starter(ent ent, ent entAnother = default, int behName = default){ yield return 0; }
	
	public override void HandleEvents()
	{
		foreach (ent entity in source.added)
		{
			ref var cAI = ref entity.ComponentAI();
			
			// добавляем стартер, если его нет#1#
			ref var behLast = ref cAI.arrBeh[cAI.arrBehIndexMax - 1];
			if (behLast.nameTag != Tag.AI_Starter)
			{   
				ref var behStarter = ref cAI.AddBehaviour(Tag.AI_Starter, (entWith, entAnother, behName) => AI_Starter(entWith, entAnother, behName));
				behStarter.triggerOn = true;
			}
			else
				behLast.triggerOn = true;
			
			for (sbyte t = 0; t < cAI.arrBehIndexMax; t++)
			{
				ref var beh = ref cAI.arrBeh[t];
				if (beh.predicateTrigger == null && beh.enumeratorTrigger == null) beh.predicateTrigger = ent => false;
				
				//заполняем киллы..
				if (beh.Kill == null) beh.Kill = ent => !ent.IsRunningBehaviour();
				
				//запускаем эвейк поведений..
				if (beh.AwakeBehaviour != null) beh.AwakeBehaviour(entity);
			}
			
			cAI.indexActive = cAI.arrBehIndexMax - 1;
		}
	}
	

	public void Tick(float delta)
	{

		for (int i = 0; i < source.length; i++)
		{
			ref var entity = ref source.entities[i];
			var cAI = entity.ComponentAI();
			
			if (cAI.prioritetAI == 127) continue;

			for (var j = cAI.arrBehIndexMax - 1; j > cAI.prioritetAI && j >= 0; j--)
			{
				ref var beh = ref cAI.arrBeh[j];
				
				switch (beh.triggerOn)
				{
					case true:
						beh.triggerOn = false;
						cAI.prioritetAI = j;
						cAI.step = 1;

						break;
					
						default:
							switch (beh.predicateTrigger != null)
							{
								case true:
									if (!beh.predicateTrigger(entity)) break;
									cAI.prioritetAI = j;
									cAI.step = 1;
									break;
								
								default:
									if (beh.triggerHandle.isRunning) break;
									//beh.triggerHandle = Timing.RunCoroutine(beh.enumeratorTrigger(entity, beh.nameTag));
									beh.triggerHandle = routines.run(beh.enumeratorTrigger(entity, beh.nameTag));
									break;
							}
							break;
				}
			}
		

			
			ref var behCurrent = ref cAI.arrBeh[cAI.indexActive];

			switch (cAI.step)
			{
				case 0:
					break;
				
				case 1:
					// отключить все триггеры корутины. приоритет которых ниже того,кто начал работу
					for (int j = 0; j < cAI.prioritetAI; j++)
					{
						ref var behTriggerHandle = ref cAI.arrBeh[j];
						
						if (behTriggerHandle.triggerHandle.isRunning)
						{
							behTriggerHandle.triggerHandle.stop();
                            //behTriggerHandle.KillCoroutines(behTriggerHandle.triggerHandle);
						}
					}
					
					if (cAI.arrBeh[cAI.prioritetAI].onTimerKillPreviousBehaviour) cAI.timeToAllNextBeh.Plus(Time.delta);
					if (behCurrent.nameTag != Tag.AI_Starter && !behCurrent.Kill(entity)) break;
					cAI.step++;
					break;
					
				case 2:
					// Запускаем поведение
					cAI.indexActive = cAI.prioritetAI;
					behCurrent = ref cAI.arrBeh[cAI.indexActive];
					//behCurrent.behaviourHandle = Timing.RunCoroutine(behCurrent.enumeratorBehaviour(entity, behCurrent.entAnother, behCurrent.nameTag));
					behCurrent.behaviourHandle = routines.run(behCurrent.enumeratorBehaviour(entity, behCurrent.entAnother, behCurrent.nameTag));

					
					if (behCurrent.onTimerKillPreviousBehaviour) cAI.timeToAllNextBeh = 0f;
					cAI.step++;
					break;
				
				case 3:
					// ждем когда поведение закончит работу и перезапускаем его, если оно циклично
					if (behCurrent.behaviourHandle.isRunning) break;
					if (cAI.prioritetAI != 127) cAI.prioritetAI = -1;
					if (behCurrent.nameTag == Tag.AI_Death)  cAI.prioritetAI = 127;
					cAI.step = 0;
					break;
			
			}
			
			
		}
	}
	
}


#region UTILITIES


public static partial class UtilsProcessor
{
	

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool EnableBehaviour(this in ent entity, in int nameBehaviour, in ent entAnother = default)
	{
		var cAI = entity.ComponentAI();

		if (cAI.prioritetAI == 127 && nameBehaviour == Tag.AI_Starter)
		{
			cAI.prioritetAI = cAI.arrBehIndexMax - 2;
			return true;
		}


		for (int i = 0; i < cAI.arrBehIndexMax; i++)
		{
			ref var beh = ref cAI.arrBeh[i];
			if (beh.nameTag != nameBehaviour) continue;

			if (entAnother != default)
			{
				beh.entAnother = entAnother;
			}
			
			if (cAI.prioritetAI == 127) Debug.Log($"!!! перед поведением не был запущен AI_Starter");
			
			beh.triggerOn = true;
			return true;

		}
		return false;
	}

	
}


#endregion
