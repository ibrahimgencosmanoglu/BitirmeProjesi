using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyAI))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyAI fov = (EnemyAI)target;
        Handles.color = Color.black;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.yellowZoneRadius);

        Handles.color = Color.red;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.redZoneRadius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.yellowZoneRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.yellowZoneRadius);

    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees) 
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
