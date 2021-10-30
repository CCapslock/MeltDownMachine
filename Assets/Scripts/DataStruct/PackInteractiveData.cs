using UnityEngine;

namespace Mans
{
    public sealed class PackInteractiveData
    {
        public readonly int attackPower;
        public readonly TypeUnit typeUnit;
        public readonly int NumCfg;
        public PackInteractiveData(int attackPower, TypeUnit typeItem,int NumCfg)
        {
            this.attackPower = attackPower;
            this.typeUnit = typeItem;
            this.NumCfg = NumCfg;
        }
    }
}