using Avalonia.Collections;
using Avalonia.Styling;
using System;
using System.Collections;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonTab : TabItem, IStyleable
    {
        Type IStyleable.StyleKey => typeof(RibbonTab);

        public static readonly DirectProperty<RibbonTab, IEnumerable> GroupsProperty = AvaloniaProperty.RegisterDirect<RibbonTab, IEnumerable>(nameof(Groups), o => o.Groups, (o, v) => o.Groups = v);

        private IEnumerable _groups = new AvaloniaList<object>();
        public IEnumerable Groups
        {
            get { return _groups; }
            set { SetAndRaise(GroupsProperty, ref _groups, value); }
        }
    }
}
