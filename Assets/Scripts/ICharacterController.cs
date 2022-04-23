using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController
{
    public void Move(bool follow);
    public void MeleeAttack(float damage);
    public void RangeAttack(float damage);
    public void RadioAttack(float damage);
}
