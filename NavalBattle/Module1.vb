Imports Microsoft.Xna.Framework

Module Module1

    Sub Main()
        Dim game As NavalBattle = New NavalBattle(New Vector2(512, 448))
        game.Run()
        game.Dispose()
    End Sub

End Module
