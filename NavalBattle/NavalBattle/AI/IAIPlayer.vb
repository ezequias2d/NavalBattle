Public Interface IAIPlayer

    Sub StartAttackProcessing(map As HouseStatus(), width As Integer, height As Integer)

    Function IsProcessingComplete() As Boolean

    Function IsInProcessing() As Boolean

    Function NextResult() As (x As Integer, y As Integer)
End Interface
