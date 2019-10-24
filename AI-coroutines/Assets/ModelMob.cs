using Pixeye.Actors;
using UnityEngine;
//using MoreLinq;

sealed partial class Model
{
    public static void Mob(in ent entity)
    {
        entity.Set<ComponentMob>();
        
        ModelAI.MobAI(entity);
        

    }


    
}
