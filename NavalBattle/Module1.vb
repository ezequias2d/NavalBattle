
Imports System.Runtime.CompilerServices

Module Module1

    Sub Main()
        Dim multiSounds As MultiSounds
        multiSounds = New MultiSounds
        multiSounds.AddSound("somdefundo", "C:\Users\Luna\Desktop\NavalBattle\NavalBattle\somdefundo.wav")
        Dim game As Game = New Game()
        multiSounds.Play("somdefundo")
        game.Run()
        game.Dispose()
    End Sub

End Module
