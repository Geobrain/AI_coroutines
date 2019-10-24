using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pixeye.Actors;
using Time = Pixeye.Actors.time;

public sealed class ComponentAI
{

	public bool @continue;
	public bool Continue()
	{
		if (!@continue) return false;
		@continue = false;
		return true;
	}
	
	public Behaviour[] arrBeh = new Behaviour[10];
	public int arrBehIndexMax;
	
	public int indexActive;
	
	//параметры текущего поведения..
	public int prioritetAI = 127;
	public byte step;
	
	public float timeToAllNextBeh;
}

#region UTILITIES

public static partial class UtilsComponent
{
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsRunningBehaviour(this in ent entity)
    {
    	var cAI = entity.ComponentAI();
        return cAI.arrBeh[cAI.indexActive].behaviourHandle.isRunning;
    }

	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void KillBehaviour(this in ent entity)
	{
		var cAI = entity.ComponentAI();
		cAI.arrBeh[cAI.indexActive].behaviourHandle.stop();
	}
	
	
}



#endregion




#region HELPERS


static partial class component
{
	public const string AI = "ComponentAI";

	public static ref ComponentAI ComponentAI(in this ent entity)
		=> ref StorageComponentAI.components[entity.id];
}

sealed class StorageComponentAI : Storage<ComponentAI>
{
	public override ComponentAI Create() => new ComponentAI();

	public override void Dispose(indexes disposed)
	{
		foreach (var id in disposed)
		{
			ref var component = ref components[id];
			component.@continue = false;
			component.arrBeh = new Behaviour[10];
			component.arrBehIndexMax = 0;
			component.indexActive = 0;
			component.prioritetAI = 127;
			component.step = 0;
			component.timeToAllNextBeh = 0;
		}
	}
}


#endregion


public struct Behaviour
{
	// название
    public int nameTag;
	
	// первичные постоянные настройки поведения
	public Action<ent> AwakeBehaviour;
	
		// триггер поведения - либо предикатор, либо корутина-триггер
    public Predicate<ent> predicateTrigger;
    
    // енумератор триггера
	public Func<ent, int, IEnumerator> enumeratorTrigger;
	
	// енумератор поведения
	public Func<ent, ent, int, IEnumerator> enumeratorBehaviour;
	// Таргет для сущности - только если нужен
	public ent entAnother;
	
	
	// способ уничтожение поведения
	public Predicate<ent> Kill;
	
	
	// временные поля
	public coroutine triggerHandle;
	public coroutine behaviourHandle;
	
	
	public bool triggerOn;
	
	// особые настройки
    public bool onTimerKillPreviousBehaviour; // поведение учитывает время окончания работы предыдущего поведения
}


public static partial class UtilsStruct
{


	static Behaviour Beh()
	{
		Behaviour newBeh;
		newBeh.AwakeBehaviour = default;
		newBeh.predicateTrigger = default;
		newBeh.enumeratorTrigger = default;
		newBeh.Kill = default;
		newBeh.triggerHandle = default;
		newBeh.behaviourHandle = default;
		newBeh.triggerOn = default;
		//ent entity = -1;
		newBeh.entAnother = -1;
		newBeh.onTimerKillPreviousBehaviour = default;

		newBeh.nameTag = default;
		newBeh.enumeratorBehaviour = default;
		
		return newBeh;
	}
	
	public static ref Behaviour AddBehaviour(this ComponentAI cAI, in int nameTag, Func<ent, ent, int, IEnumerator> enumeratorBehaviour)
	{
		var newBeh = Beh();
		
		newBeh.nameTag = nameTag;
		newBeh.enumeratorBehaviour = enumeratorBehaviour;
		//buffer[bufferIndex] = newSr;
		cAI.arrBeh[cAI.arrBehIndexMax] = newBeh;
		ref var beh = ref cAI.arrBeh[cAI.arrBehIndexMax];
		cAI.arrBehIndexMax++;
		
		return ref beh;
	}

}

