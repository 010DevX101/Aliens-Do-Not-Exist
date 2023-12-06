using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000016 RID: 22
public class MutantLogic : MonoBehaviour
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000059 RID: 89 RVA: 0x000036B3 File Offset: 0x000018B3
	// (set) Token: 0x0600005A RID: 90 RVA: 0x000036BA File Offset: 0x000018BA
	public static MutantLogic Instance { get; private set; }

	// Token: 0x0600005B RID: 91 RVA: 0x000036C2 File Offset: 0x000018C2
	private void Awake()
	{
		MutantLogic.Instance = this;
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000036CA File Offset: 0x000018CA
	private void Start()
	{
		this._playerTransform = References.Instance.PlayerGO.transform;
		this._collider = base.GetComponent<Collider>();
		this._rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600005D RID: 93 RVA: 0x000036FC File Offset: 0x000018FC
	private void Update()
	{
		if (!this._onJumpAttack && !this._falling)
		{
			this._navMeshAgent.SetDestination(this._playerTransform.position);
			Vector3 vector = base.transform.position + Vector3.up;
			Debug.DrawLine(vector, this._playerTransform.position, Color.red);
			RaycastHit raycastHit;
			if (Physics.Raycast(vector, this._playerTransform.position - vector, ref raycastHit, this._distance) && raycastHit.transform.CompareTag("Player"))
			{
				this._animator.SetTrigger("JumpAttack");
				this._navMeshAgent.speed = this._speedInJumpAttack;
				this._onJumpAttack = true;
			}
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x000037BE File Offset: 0x000019BE
	public void JumpAttackAnimFinished()
	{
		if (!this._falling)
		{
			this._animator.SetTrigger("Run");
			this._navMeshAgent.speed = this._normalSpeed;
			this._onJumpAttack = false;
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000037F0 File Offset: 0x000019F0
	public void Fall()
	{
		if (!this._falling)
		{
			this._collider.enabled = false;
			this._animator.SetTrigger("JumpAttack");
			this._rigidbody.constraints -= 2;
			this._rigidbody.constraints -= 8;
			base.StartCoroutine(this.FallAnim());
			this._falling = true;
			this._navMeshAgent.SetDestination(References.Instance.VacioTransform.position);
			this._navMeshAgent.enabled = false;
			this._rigidbody.AddForce(base.transform.forward * 10f + Vector3.up * 10f, 1);
			VolverAlMenu.Instance.PlayAnim();
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000038C4 File Offset: 0x00001AC4
	private IEnumerator FallAnim()
	{
		yield return new WaitForSeconds(1f);
		this._animator.SetTrigger("Fall");
		this._rigidbody.AddForce(base.transform.forward + Vector3.up, 1);
		yield break;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x000038D3 File Offset: 0x00001AD3
	public void PlayExplosionSound()
	{
		SoundsController.Instance.PlayExplosion();
	}

	// Token: 0x0400006B RID: 107
	[SerializeField]
	private NavMeshAgent _navMeshAgent;

	// Token: 0x0400006C RID: 108
	[SerializeField]
	private Animator _animator;

	// Token: 0x0400006D RID: 109
	[SerializeField]
	private float _distance;

	// Token: 0x0400006E RID: 110
	[SerializeField]
	private float _normalSpeed = 6f;

	// Token: 0x0400006F RID: 111
	[SerializeField]
	private float _speedInJumpAttack = 8f;

	// Token: 0x04000070 RID: 112
	private Collider _collider;

	// Token: 0x04000071 RID: 113
	private Rigidbody _rigidbody;

	// Token: 0x04000072 RID: 114
	private Transform _playerTransform;

	// Token: 0x04000073 RID: 115
	private bool _onJumpAttack;

	// Token: 0x04000074 RID: 116
	private bool _falling;
}
