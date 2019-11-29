
Imports System.Runtime.CompilerServices

Module Module1

    Sub Main()
        Dim multiSounds As MultiSounds
        multiSounds = New MultiSounds
        multiSounds.AddSound("bgSound", "..\Data\somdefundo.wav")
        Dim game As Game = New Game()
        multiSounds.Play("bgSound")
        game.Run()
        game.Dispose()
    End Sub

End Module
