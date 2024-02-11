using System;
using System.Collections;
using Sources.ControllersInterfaces;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.GameObject;
using Sources.Services.Triggers;
using Sources.UseCases.Scene;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.Player
{
	public class PlayerBody : Presenter, IPlayer { }
}