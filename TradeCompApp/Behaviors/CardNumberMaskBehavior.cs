using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCompApp.Behaviors
{
    class CardNumberMaskBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = sender as Entry;
            var text = entry.Text?.Replace(" ", "") ?? "";

            if (text.Length > 16) text = text.Substring(0, 16);

            var formatted = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (i > 0 && i % 4 == 0) formatted.Append(" ");
                formatted.Append(text[i]);
            }

            entry.Text = formatted.ToString();
        }
    }
}
