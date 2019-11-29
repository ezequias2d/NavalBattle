Public Interface IAIMap

    Function GenereteMap(width As UInteger,
                         height As UInteger,
                         battleship As UInteger,
                         carrier As UInteger,
                         destroyer As UInteger,
                         submarine As UInteger) As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation)()
End Interface
