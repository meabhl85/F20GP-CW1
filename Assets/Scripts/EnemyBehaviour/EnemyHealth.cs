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

		public bool smEnemy = true;

		void OnEnable()
		{
			currentHealth = startingHealth;
			//animator = GetComponent<Animator>();
			rend = GetComponent<Renderer>();
			enemyScript = GetComponent<Enemy>();
		}

		public void TakeDamage(int damageAmount)
		{
			currentHealth -= damageAmount;
			if (currentHealth <= 0 && currentHealth > -0.1)
				Die();
		}

		private void Die()
		{
			//Update Player score
			score.UpdateScore(100);

			//Set Enemy to black material
			rend.material.SetColor("_Color", Color.black);

			if (smEnemy)
            {
				enemyScript.alive = false;
				enemyScript._stateMachine.ChangeState(enemyScript.idle);
			}
			
			gameManager.KilledEnemy(gameObject);

			Destroy(gameObject, 2.0f);
		}


	}
}
