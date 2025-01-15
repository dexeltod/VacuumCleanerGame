using Sources.Domain.Interfaces;
using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeWindowActivator
	{
		GameObject Container { get; }
		ITextPhrases PhrasesList { get; }
	}
}