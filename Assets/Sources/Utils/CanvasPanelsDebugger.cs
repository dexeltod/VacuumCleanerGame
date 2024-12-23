using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Sources.Utils
{
    public class CanvasPanelsDebugger : MonoBehaviour
    {
        [SerializeField] private GraphicRaycaster _graphicRaycaster;
        private EventSystem _eventSystem;
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(1);

        private void OnEnable()
        {
            _eventSystem = EventSystem.current;
            _coroutine = StartCoroutine(StartDebug());
        }

        private void OnDestroy()
        {
            _eventSystem = EventSystem.current;

            if (_coroutine != null) StopCoroutine(_coroutine);
        }


        private IEnumerator StartDebug()
        {
            while (true)
            {
                var context = new InputAction();


                // Создаем PointerEventData с текущим положением мыши
                List<RaycastResult> results = new List<RaycastResult>();


                PointerEventData pointerEventData = new PointerEventData(_eventSystem)
                {
                    position = Input.mousePosition
                };


                // Создаем список для сохранения результатов

                // Делаем raycast
                _graphicRaycaster.Raycast(pointerEventData, results);

                if (results is { Count: > 0 })
                {
                    Debug.Log("UI Element: " + results[0].gameObject.name);

                    results.Clear();
                }

                yield return _waitForSeconds;
            }
        }
    }
}