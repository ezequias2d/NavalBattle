Imports Microsoft.Xna.Framework
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' AnimatedSprite.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Sprite animado
''' </summary>
''' <typeparam name="Key"> Identificador de animação </typeparam>
Public Class AnimatedSprite(Of Key)
    Inherits Sprite
    Implements IUpdate

    ''' <summary>
    ''' Velocidade de troca de Sprite.
    ''' </summary>
    ''' <returns></returns>
    Public Property UpdatesPerSecond As Single
        Get
            Return _UpdatesPerSecond
        End Get
        Set
            If Value > 0F Then
                _UpdatesPerSecond = Value
                _InversUpdatePerSecond = 1.0F / Value
            Else
                Throw New ArgumentOutOfRangeException("UpdatePerSecond", _UpdatesPerSecond, "Updates per second must be greater than zero. If you want to stop the update, set UpdateEnable to false.")
            End If
        End Set
    End Property

    ''' <summary>
    ''' Determina se está ativado a atualização.
    ''' </summary>
    ''' <returns> Atualização ativada </returns>
    Public Property UpdateEnable As Boolean Implements IUpdate.UpdateEnable
        Get
            Return _UpdateEnable
        End Get
        Set(value As Boolean)
            _UpdateEnable = value
        End Set
    End Property

    ''' <summary>
    ''' Animações em dicionario.
    ''' </summary>
    ''' <returns> Animações </returns>
    Public Property Animations As IDictionary(Of Key, Animation)
        Get
            Return _Animations
        End Get
        Set
            If Value Is Nothing Then
                Throw New ArgumentNullException("Animations", "Animations cannot be null.")
            Else
                _Animations = Value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Contador de atualização de Frame.
    ''' </summary>
    Private count As Single

    ''' <summary>
    ''' Numero de atualizações por segundo.
    ''' </summary>
    Private _UpdatesPerSecond As Single

    ''' <summary>
    ''' Numero de atualizações por segundo invertido( 1 / _UpdatesPerSecond ).
    ''' 
    ''' Usado para economizar recursos, reduzindo a quantidade de operações na ULA.
    ''' </summary>
    Private _InversUpdatePerSecond As Single

    Private _UpdateEnable As Boolean
    Private _Animations As IDictionary(Of Key, Animation)
    Public animation As Key

    ''' <summary>
    ''' Cria novo AnimatedSprite.
    ''' </summary>
    ''' <param name="position"> Posição </param>
    ''' <param name="updatesPerSecond"> Atualizações por segundo de Sprite </param>
    ''' <param name="animations"> Animations </param>
    Public Sub New(position As Vector2, updatesPerSecond As Single, ByRef animations As IDictionary(Of Key, Animation))
        MyBase.New(position)
        Me.UpdateEnable = True

        Try
            Me.UpdatesPerSecond = updatesPerSecond
        Catch ex As ArgumentOutOfRangeException
            Me.UpdatesPerSecond = 5.0F
            Me.UpdateEnable = False
        End Try

        Try
            Me.Animations = animations
        Catch ex As ArgumentNullException
            Me.Animations = New Dictionary(Of Key, Animation)
        End Try
    End Sub

    ''' <summary>
    ''' Cria novo AnimatedSprite.
    ''' </summary>
    ''' <param name="position"> Posição </param>
    ''' <param name="updatesPerSecond"> Atualizações por segundo </param>
    Public Sub New(position As Vector2, updatesPerSecond As Single)
        Me.New(position, updatesPerSecond, New Dictionary(Of Key, Animation))
    End Sub

    ''' <summary>
    ''' Cria novo AnimatedSprite.
    ''' </summary>
    ''' <param name="position"> Posição </param>
    ''' <param name="animations"> Animações </param>
    Public Sub New(position As Vector2, ByRef animations As IDictionary(Of Key, Animation))
        Me.New(position, 5.0F, animations)
    End Sub

    ''' <summary>
    ''' Cira novo AnimatedSprite
    ''' </summary>
    ''' <param name="position"> Posição </param>
    Public Sub New(position As Vector2)
        Me.New(position, 5.0F, New Dictionary(Of Key, Animation))
    End Sub

    ''' <summary>
    ''' Cira novo AnimatedSprite
    ''' </summary>
    Public Sub New()
        Me.New(Vector2.Zero, 5.0F, New Dictionary(Of Key, Animation))
    End Sub

    ''' <summary>
    ''' Update.
    ''' </summary>
    ''' <param name="gameTime"> GameTime </param>
    Public Sub Update(ByVal gameTime As GameTime) Implements IUpdate.Update
        If Animations.Count = 0 Then
            Return
        End If

        count += gameTime.ElapsedGameTime.TotalSeconds
        If (count > _InversUpdatePerSecond) Then
            count -= _InversUpdatePerSecond

            Dim ani As Animation = Animations(animation)
            ani.NextFrame()
            Frame = ani.Current
        End If
    End Sub

    ''' <summary>
    ''' Seleciona Frame, utilizando o index, da animação atual.
    ''' </summary>
    ''' <param name="index"> Index </param>
    Public Sub SetFrameIndex(ByVal index As Integer)
        Dim ani As Animation = Animations(animation)
        ani.SetFrameIndex(index)
        Frame = ani.Current
    End Sub

End Class
