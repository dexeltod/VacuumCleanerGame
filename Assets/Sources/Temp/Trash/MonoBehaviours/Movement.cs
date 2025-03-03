using UnityEngine;

namespace Temp.Trash.MonoBehaviours
{
	public class Movement : MonoBehaviour
	{
		private readonly float _rotationSpeed = 100f; // Скорость вращения
		private readonly float _speed = 5f; // Скорость перемещения
		private Rigidbody _body; // Ссылка на Rigidbody

		private void Start()
		{
			// Получаем компонент Rigidbody (если он есть)
			_body = GetComponent<Rigidbody>();

			// Если Rigidbody отсутствует, добавляем его.  Рекомендуется добавить Rigidbody к вашему игровому объекту в редакторе.
			if (_body == null)
			{
				_body = gameObject.AddComponent<Rigidbody>();
				_body.freezeRotation = true; // Замораживаем вращение, чтобы не было нежелательных вращений.
			}
		}

		private void Update()
		{
			// Обработка ввода для перемещения и вращения
			float horizontalInput = Input.GetAxis("Horizontal");
			float verticalInput = Input.GetAxis("Vertical");

			// Перемещение по осям X и Z (плоскость)
			Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

			// Плавное перемещение с использованием Time.deltaTime (обновляется каждый кадр)
			// Без Time.deltaTime перемещение будет зависеть от частоты кадров, и на разных компьютерах будет разным.
			transform.Translate(movement * _speed * Time.deltaTime, Space.World);

			// Вращение (для примера, только вращение по Y оси)
			float rotationInput =
				Input.GetAxis("Rotation"); // Предполагается, что у вас есть ось ввода Rotation.  Можно использовать любую другую.
			transform.Rotate(Vector3.up, rotationInput * _rotationSpeed * Time.deltaTime);
		}

		private void FixedUpdate()
		{
			// Здесь должна быть логика, связанная с физикой. Например, приложение силы.
			// Обратите внимание, что мы НЕ используем Time.deltaTime здесь.  FixedDeltaTime автоматически применяется.

			// Пример применения силы с использованием ввода в FixedUpdate.
			// Этот пример демонстрирует, как Time.fixedDeltaTime подразумевается в FixedUpdate.
			float horizontalInput = Input.GetAxis("Horizontal");
			float verticalInput = Input.GetAxis("Vertical");

			Vector3 force = new Vector3(horizontalInput, 0f, verticalInput).normalized * _speed;

			_body.AddForce(force);
		}
	}
}
