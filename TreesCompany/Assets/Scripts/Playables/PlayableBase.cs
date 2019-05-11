using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Playables
{
    public class PlayableBase : MonoBehaviour
    {
        public int Height { get; } = 1;

        public virtual int TreeCost { get; } = 1;

        public virtual int PowerCost { get; } = 1;
    }
}
