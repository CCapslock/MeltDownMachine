using UnityEngine;

namespace Mans
{
    public sealed class PackInteractiveData
    {
        public int attackPower { get; }
        public TypeUnit typeUnit { get; }
        public PackInteractiveData(int attackPower, TypeUnit typeItem)
        {
            this.attackPower = attackPower;
            this.typeUnit = typeItem;            
        }
    }
}