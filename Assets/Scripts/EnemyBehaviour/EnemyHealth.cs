using UnityEngine;

namespace Assets.Scripts.EnemyBehaviour
{
	public class EnemyHealth : MonoBehaviour
	{
		[SerializeField]
		private int startingHealth = 5;

		public int currentHealth;

		public Score score;
		private Animator animator;
		public GameManager gameManager;
		Enemy enemyScript;
		Renderer rend;

		void OnEnable()
		{
			currentHealth = startingHealth;
			//animator = GetComponent<Animator>();
			rend = GetComponent<Renderer>();
			enemyScript = this.GetComponent<Enemy>();
		}

		public void TakeDamage(int damageAmount)
		{
			currentHealth -= damageAmount;
			if (currentHealth <= 0 && currentHealth > -0.1)
				Die();
		}

		private void Die()
		{
			//animator.SetBool("Dead", true);
			score.UpdateScore(100);

			rend.material.SetColor("_Color", Color.black);

			enemyScript.alive = false;
			enemyScript._stateMachine.ChangeState(enemyScript.idle);
			gameManager.KilledEnemy(gameObject);

			Destroy(gameObject, 3.0f);
		}


	}
}
