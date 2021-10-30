using UnityEngine;

namespace Mans
{

    public sealed class UnitViewTraectory : MonoBehaviour, ITraectory
    {
        private Color _clr = Color.red;
        private float _sizeTrack = 0.05f;

        [SerializeField] private Traectory[] _track;
        public Traectory[] Track => _track;

        private void OnDrawGizmos()
        {
            if (_track == null || _track.Length <= 1) return;

            for (int i = 1; i < _track.Length; i++)
            {
                if (!trackIsRelevant(i)) continue;
                Utils.LineGizmo(_track[i - 1].transform.position, _track[i].transform.position, _sizeTrack, _clr);
            }

            if (_track[_track.Length - 1].transform != null && _track[0].transform != null)
                Utils.LineGizmo(_track[_track.Length - 1].transform.position, _track[0].transform.position, _sizeTrack, _clr);

            bool trackIsRelevant(int i)
            {
                return _track[i - 1].transform != null
                        && _track[i].transform != null
                        && _track[i - 1].transform != _track[i].transform;
            }
        }
    }
}