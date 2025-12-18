using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAugment : MonoBehaviour
{
    [SerializeField] private List<Augment> _equippedAugments = new List<Augment>();
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private TextMeshProUGUI[] _augmentsText;
    private int _oldestIndex = 0;
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
            Augment oldAugment = _equippedAugments[_oldestIndex];

            _equippedAugments.Remove(oldAugment);

            _equippedAugments[_oldestIndex] = augment;
            _playerStats.EquipAugment(augment);
            _augmentsText[_oldestIndex].text = augment.GetAugmentName();

            _oldestIndex = (_oldestIndex + 1) % _maxAugment;
        }
    }


    public List<Augment> GetEquippedAugments()
    {
        return _equippedAugments;
    }
}
