using System;
using System.Runtime.CompilerServices;
using Pixeye.Actors;
using UnityEngine;


public sealed class ComponentMove
{
    //public Legat legat;
	//public Agent agent;
	public Vector3 direction;

}

#region UTILITIES

public static partial class UtilsComponent
{
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetCacheEnable(this ComponentMove component, in ent entity)
    {
        //component.collBox = entity.transform.GetComponent<BoxCollider2D>();
    }	
}

#endregion

#region HELPERS

static partial class component
	{
		public const string Move = "ComponentMove";
		public static ref ComponentMove ComponentMove(in this ent entity) => ref StorageComponentMove.components[entity.id];
	}

	sealed class StorageComponentMove : Storage<ComponentMove>
	{
		public override ComponentMove Create() => new ComponentMove();
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

