using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using TMPro;

public class UIUltimateWeapon : UIEntity
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse ultimateEnabledListener;
	public EventListenerDelegateResponse ultimateUsedListener;

	public SharedFloatProperty ultimateProgressProperty;

	// [Header( "Fired Events" )]

	[Header( "UI Elements" )]
	public TextMeshProUGUI progressText;
	public Image progressImage;

	// Private Fields
	private Button selectionButton;
	#endregion

	#region UnityAPI
	private void OnEnable()
	{
		ultimateEnabledListener.OnEnable();
		ultimateUsedListener.OnEnable();

		ultimateProgressProperty.changeEvent += ProgressChangeResponse;
	}

	private void OnDisable()
	{
		ultimateEnabledListener.OnDisable();
		ultimateUsedListener.OnDisable();
		
		ultimateProgressProperty.changeEvent -= ProgressChangeResponse;
	}

	private void Awake()
	{
		selectionButton = GetComponent<Button>();

		selectionButton.interactable = false; // just in case
		progressImage.fillAmount = 0;

		ultimateEnabledListener.response = () => selectionButton.interactable = true;
		ultimateUsedListener.response    = UltimateUsedResponse;
	}
	#endregion

	#region Implementation
	void ProgressChangeResponse()
	{
		progressText.text = "%" + (int) ultimateProgressProperty.sharedValue ;
		progressImage.fillAmount = ultimateProgressProperty.sharedValue / 100f;
	}

	void UltimateUsedResponse()
	{
		progressText.text = "%" + 0;
		progressImage.fillAmount = 0;
		selectionButton.interactable = false;
	}
	#endregion
}
