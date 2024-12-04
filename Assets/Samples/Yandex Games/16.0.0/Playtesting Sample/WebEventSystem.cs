using UnityEngine.EventSystems;

namespace Samples.Yandex_Games._16._0._0.Playtesting_Sample
{
    /// <summary>
    /// Fixes unresponsive UI controls after alt-tabbing on mobile Google Chrome.
    /// </summary>
    /// <remarks>
    /// Workaround for <see href="https://trello.com/c/PjW4j3st"/>
    /// </remarks>
    public class WebEventSystem : EventSystem
    {
        protected override void OnApplicationFocus(bool hasFocus) => base.OnApplicationFocus(true);
    }
}
