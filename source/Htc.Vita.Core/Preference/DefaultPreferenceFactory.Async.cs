﻿using System.Threading.Tasks;

namespace Htc.Vita.Core.Preference
{
    public partial class DefaultPreferenceFactory
    {
        protected override Task<Preferences> OnLoadPreferencesAsync(string label)
        {
            return new DefaultPreferences("", label).InitializeAsync();
        }
    }
}
