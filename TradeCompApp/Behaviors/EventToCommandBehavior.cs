using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TradeCompApp.Models.Behaviors
{
    public class EventToCommandBehavior : Behavior<CheckBox>
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior));

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior));

        public static readonly BindableProperty EventNameProperty =
            BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommandBehavior));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        protected override void OnAttachedTo(CheckBox bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.CheckedChanged += OnCheckedChanged;
        }

        protected override void OnDetachingFrom(CheckBox bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.CheckedChanged -= OnCheckedChanged;
        }

        private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Command?.CanExecute(CommandParameter) ?? false)
            {
                Command.Execute(CommandParameter ?? sender);
            }
        }
    }
}
