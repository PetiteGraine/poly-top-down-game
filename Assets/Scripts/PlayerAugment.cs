using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAugment : MonoBehaviour
{
    [SerializeField] private List<Augment> _equippedAugments = new List<Augment>();
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private TextMeshProUGUI[] _augmentsText;
    private int _oldestAugmentIndex = 0;
    private int _maxAugment = 3;

    private void Start()
    {
        if (_playerStats == null)
        {
            _playerStats = GetComponent<PlayerStats>();
        }
    }

    public void EquipAugment(Augment augment)
    {
        if (_equippedAugments.Exists(a => a.GetAugmentName() == augment.GetAugmentName()))
        {
            return;
        }

        if (_equippedAugments.Count < _maxAugment)
        {
            _equippedAugments.Add(augment);
            _playerStats.EquipAugment(augment);
            _augmentsText[_equippedAugments.Count - 1].text = augment.GetAugmentName();
        }

        else
        {
            _equippedAugments.RemoveAt(_oldestAugmentIndex);
            _equippedAugments.Insert(_oldestAugmentIndex, augment);
            _playerStats.EquipAugment(augment);
            _augmentsText[_oldestAugmentIndex].text = augment.GetAugmentName();
            _oldestAugmentIndex = (_oldestAugmentIndex + 1) % _maxAugment;
        }
    }


    public List<Augment> GetEquippedAugments()
    {
        return _equippedAugments;
    }
}
