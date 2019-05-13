using System.Collections;
using System.Collections.Generic;
using Assets.Playables;
using UnityEngine;

public class TreePlayable : PlayableBase
{
    public override int TreeCost => 1;
    public override int PowerCost => 0;
}