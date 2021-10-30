using System;

namespace Mans
{
    public sealed class ListControllers 
    {
        private static event Action _init = delegate { };
        private static event Action<float> _execute = delegate { };
        private static event Action<float> _fixedExecute = delegate { };
        private static event Action<float> _lateExecute = delegate { };
        private static bool isInitHasPassed = false;
        public static int countAddListControllers = 0;

        public static void Init()
        {
        _init = delegate { };
        _execute = delegate { };
        _lateExecute = delegate { };
        _fixedExecute = delegate { };
        isInitHasPassed = false;
        countAddListControllers = 0;
        }

        public static void Execute(float deltaTime)
        {
            _execute(deltaTime);
        }

        internal static void FixedExecute(float deltaTime)
        {
            _fixedExecute(deltaTime);
        }

        public static void Initialization()
        {
            isInitHasPassed = true;
            _init();
        }

        public static void LateExecute(float deltaTime)
        {
            _lateExecute(deltaTime);
        }

        public static void Add(IController controller, string name = "")
        {
            countAddListControllers++;
            if (controller is IInitialization init)
            {
                _init += init.Initialization;
                if (isInitHasPassed) init.Initialization();
            }
            if (controller is IExecute execute)
            {
                _execute += execute.Execute;
            }
            if (controller is IFixedExecute fixedExecute)
            {
                _fixedExecute += fixedExecute.FixedExecute;
            }
            if (controller is ILateExecute lateExecute)
            {
                _lateExecute += lateExecute.LateExecute;
            }
        }

        public static void Delete(IController controller)
        {
            countAddListControllers--;
            if (controller is IInitialization init)
            {
                _init -= init.Initialization;
            }
            if (controller is IExecute execute)
            {
                _execute -= execute.Execute;
            }
            if (controller is IFixedExecute fixedExecute)
            {
                _fixedExecute -= fixedExecute.FixedExecute;
            }
            if (controller is ILateExecute lateExecute)
            {
                _lateExecute -= lateExecute.LateExecute;
            }
        }
    }
}
