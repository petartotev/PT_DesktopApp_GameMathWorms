using GameMathWorms.Constants;
using GameMathWorms.Enums;
using System;
using System.Windows.Controls;

namespace GameMathWorms.Models
{
    public class Target
    {
        private readonly Random _random = new Random();

        public Target(Label label)
        {
            Label = label;
        }

        public int Value { get; private set; }

        public TargetOperationEnum Operation { get; private set; }

        public Label Label { get; }

        public void SetRandomly()
        {
            SetRandomOperation();
            SetValueByOperation();

            Label.Content = this.ToString();
        }

        public override string ToString()
        {
            return Operation switch
            {
                TargetOperationEnum.None => $"{Value}",
                TargetOperationEnum.Add => $"+{Value}",
                TargetOperationEnum.Subtract => $"-{Value}",
                TargetOperationEnum.Multiply => $"*{Value}",
                TargetOperationEnum.Divide => $"/{Value}",
                _ => $"{Value}"
            };
        }

        private void SetRandomOperation()
        {
            Array enumValues = Enum.GetValues(typeof(TargetOperationEnum));

            Operation = (TargetOperationEnum)enumValues.GetValue(_random.Next(1, enumValues.Length));
        }

        private void SetValueByOperation()
        {
            Value = Operation switch
            {
                TargetOperationEnum.None => 0,
                TargetOperationEnum.Add => _random.Next(1, GameConstants.Target.OperationAddMaxValue),
                TargetOperationEnum.Subtract => _random.Next(1, GameConstants.Target.OperationSubtractMaxValue),
                TargetOperationEnum.Multiply => _random.Next(1, GameConstants.Target.OperationMultiplyMaxValue),
                TargetOperationEnum.Divide => _random.Next(1, GameConstants.Target.OperationDivideMaxValue),
                _ => 0
            };
        }
    }
}
