Imports NavalBattle

Public Structure House

    Public Sub New(ship As Ship, orientation As Orientation, status As HouseStatus)
        Me.New()
        Me.Ship = ship
        Me.Orientation = orientation
        Me.Status = status
    End Sub

    Public Property Ship As Ship
    Public Property Orientation As Orientation
    Public Property Status As HouseStatus
End Structure
