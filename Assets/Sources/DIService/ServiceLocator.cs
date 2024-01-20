// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Sources.DIService
// {
// 	public class ServiceLocator
// 	{
// 		private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();
//
// 		public static ServiceLocator Container { get; private set; }
//
// 		public static void Initialize() =>
// 			Container = new ServiceLocator();
//
// 		public T Get<T>() where T : IService
// 		{
// 			if (!_services.ContainsKey(typeof(T)))
// 			{
// 				Debug.LogError($"{typeof(T).Name} not registered with {GetType().Name}");
// 				throw new InvalidOperationException();
// 			}
//
// 			return (T)_services[typeof(T)];
// 		}
//
// 		public T Register<T>(T service) where T : IService
// 		{
// 			if (_services.ContainsKey(typeof(T)))
// 			{
// 				Debug.LogError
// 				(
// 					$"Attempted to register service of type {typeof(T).Name} which is already registered with the {GetType().Name}."
// 				);
// 				throw new InvalidOperationException();
// 			}
//
// 			_services.Add(typeof(T), service);
//
// 			return service;
// 		}
//
// 		public void Unregister<T>() where T : IService
// 		{
// 			if (!_services.ContainsKey(typeof(T)))
// 			{
// 				Debug.LogError
// 				(
// 					$"Attempted to unregister service of type {typeof(T).Name} which is not registered with the {GetType().Name}."
// 				);
// 				throw new InvalidOperationException();
// 			}
//
// 			_services.Remove(typeof(T));
// 		}
//
// 		private ServiceLocator() { }
//
// 		// private static ServiceLocator _instance;
// 		// public static  ServiceLocator Container => _instance ??= new ServiceLocator();
// 		//
// 		// public TService Register<TService>(TService implementation) where TService : IService
// 		// {
// 		// 	return Implementation<TService>.ServiceInstance = implementation;
// 		// }
// 		//
// 		// public TService Get<TService>() where TService : IService =>
// 		// 	Implementation<TService>.ServiceInstance;
// 		//
// 		// private static class Implementation<TService> where TService : IService
// 		// {
// 		// 	public static TService ServiceInstance;
// 		// }
// 	}
// }