using System;
using System.Runtime.CompilerServices;
using Pixeye.Actors;
using UnityEngine;


public sealed class ComponentMob
{
    //public Legat legat;
	//public Agent agent;
	public string aaa = "aaa";
	public string qqq = "qqq";
	public string www = "www";

}

#region UTILITIES

public static partial class UtilsComponent
{
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetCacheEnable(this ComponentMob component, in ent entity)
    {
        //component.collBox = entity.transform.GetComponent<BoxCollider2D>();
    }	
}

#endregion

#region HELPERS

static partial class component
	{
		public const string Mob = "ComponentMob";
		public static ref ComponentMob ComponentMob(in this ent entity) => ref StorageComponentMob.components[entity.id];
	}

	sealed class StorageComponentMob : Storage<ComponentMob>
	{
		public override ComponentMob Create() => new ComponentMob();
		public override void Dispose(indexes disposed)
		{
			foreach (var id in disposed)
			{
				ref var component = ref components[id];
				//component.nativeBox2DColl.Dispose();
			}
		}
	}

#endregion

