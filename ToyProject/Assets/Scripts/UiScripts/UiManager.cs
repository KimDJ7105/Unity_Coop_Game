using UnityEngine;

public class UiManager : MonoBehaviour
{

	enum UiState
	{
		None,
		SelectPanel,
		IpPanel,
		LobbyPanel,

	}

	
	//Panel
	public GameObject SelectPanel;
	public GameObject IpPanel;
	public GameObject LobbyPanel;
	

	private UiState state = UiState.None;


	private void Awake()
	{
		state = UiState.SelectPanel;
	}


	public void OnLobbyCreateButton()
	{
		Debug.Log("로비 생성 클릭");
		SelectPanel.SetActive(false);
		LobbyPanel.SetActive(true);
		state = UiState.LobbyPanel;

		//issue:기획
		//ip를 표시해야하나
		

		//issue:서버
		//로비를 만드는 함수
		//이 부분에 서버쪽 코드 필요
	}
	public void OnLobbyJoinButton()
	{
		Debug.Log("로비 참가 버튼 클릭");
		SelectPanel.SetActive(false);
		IpPanel.SetActive(true);
		state= UiState.IpPanel;

		//issue:서버
		//로비에 참가하는 함수
		//서버쪽 코드 필요

	}

	public void OnUndoButton()
	{
		Debug.Log("뒤로가기 버튼 클릭");
		switch (state)
		{
			case UiState.None:
				Debug.Log("양정우 여기 고쳐줘");
				break;

			case UiState.SelectPanel:
				Debug.Log("SelectPanel 상태");
				break;

			case UiState.IpPanel:
				Debug.Log("IpPanel 상태");
				SelectPanel.SetActive(true);
				IpPanel.SetActive(false);
				state = UiState.SelectPanel;
				break;

			case UiState.LobbyPanel:
				Debug.Log("LobbyPanel 상태");
				SelectPanel.SetActive(true);
				LobbyPanel.SetActive(false);
				state = UiState.SelectPanel;
				break;

		}
	}
}
