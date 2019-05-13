using System.Collections;
using System.Collections.Generic;
using Assets.Playables;
using UnityEngine;

public class HousePlayable : PlayableBase
{
    public override int TreeCost => 5;
    public override int PowerCost => 3;
}