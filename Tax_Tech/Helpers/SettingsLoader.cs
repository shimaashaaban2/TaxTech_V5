using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.Repository;

namespace Tax_Tech.Helpers
{
    public class SettingsLoader
    {
        private readonly SettingsApiRepository _settingsApiRepository;

        public SettingsLoader()
        {
            _settingsApiRepository = new SettingsApiRepository();
        }

        public SettingsApiModel LoadUserSettings(string userId)
        {
            var designMode = _settingsApiRepository.GetAppDesignMode(userId);
            var appLang = _settingsApiRepository.GetAppLangByUserID(userId);

            if (designMode == null || appLang == null)
            {
                return new SettingsApiModel
                {
                    IsDarkMode = false,
                    Language = "en-US"
                };
            }

            return new SettingsApiModel
            {
                IsDarkMode = designMode.IsDarkMode ?? false,
                Language = string.IsNullOrWhiteSpace(appLang.Language) ? "en-US" : appLang.Language
            };
        }
    }
}
