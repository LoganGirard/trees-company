using System.Collections;
using System.Collections.Generic;
using Assets.Playables;
using UnityEngine;

public class PowerPlantPlayable : PlayableBase
{
    public override int TreeCost => 10;
    public override int PowerCost => 10;
}