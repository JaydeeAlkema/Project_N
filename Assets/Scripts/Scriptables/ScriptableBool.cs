using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Bool", menuName = "ScriptableObjects/New Scriptable Bool")]
public class ScriptableBool : ScriptableObject
{
	public bool value;
	public bool resetOnDestroy = false;
	[ShowIf("resetOnDestroy")] public bool startValue;

	private void OnDestroy()
	{
		Reset();
	}

	private void OnEnable()
	{
		Reset();
	}

	private void OnDisable()
	{
		Reset();
	}

	public void Reset()
	{
		value = startValue;
	}
}
