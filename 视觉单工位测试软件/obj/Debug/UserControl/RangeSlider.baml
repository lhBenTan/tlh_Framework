   M S B A M L   `   `   `  ����]  Y视觉单工位测试软件, Version=1.0.8043.27058, Culture=neutral, PublicKeyToken=nullI)clr-namespace:视觉单工位测试软件视觉单工位测试软件  5      -    '视觉单工位测试软件.RangeSlider   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Z VDevExpress.Mvvm.UI, Version=21.1.0.0, Culture=neutral, PublicKeyToken=382ad28f6ae31f30W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:视觉单工位测试软件     5      -   5        $��
UC_Arrange��5      .��5
      ��   �� Orientation$  
Horizontal=�6   ��6*   ��  �� ElementName$ 
UC_Arrange�� �� Path$ SilderHeight�.+�5   
   �� -   5        $��35��5      $	��0,3q�5      
����d���5      �� $ SL_Bat1��$ Value� �� Mode$ TwoWay=�5      �� 5   
   $��0,0,5,0q�6K   ��6   �� $ 
UC_Arrange��$ SilderWidth�.+�5      �� -   5        $��SL_Bat1��5       �� IsSelectionRangeEnabled .  �� TickPlacement$ BottomRight=�5       �� IsSnapToTickEnabled . $��Center=�5      $��2q�5       � Value 5      �� $ 
UC_Arrange��$ 
StartValue� � Minimum 5      �� $ 
UC_Arrange��$ Minimum�	 � Maximum	 5      �� $ 
UC_Arrange��$ Maximum�
 �� SelectionStart
 5      �� $ 
UC_Arrange��$ 
StartValue� �� SelectionEnd 5      �� $ 
UC_Arrange��$ EndValue���5      �� $ 
UC_Arrange��$ SilderWidth� �� TickFrequency 5      �� $ 
UC_Arrange��$ SliderTickFrequency� � FocusVisualStyle 5      Z�  J� Clip 5      �� 5        �� Rect 6(   �� $ 
UC_Arrange��$ 	StartRect�5"      �� 5#      -   5        $��SL_Bat2��5#       . $ BottomRight=�5-       . $��Center=�5/      $��2q�50       5$      �� $ 
UC_Arrange��$ EndValue� 5%      �� $ 
UC_Arrange��$ Minimum�	 5&      �� $ 
UC_Arrange��$ Maximum�
 5'      �� $ 
UC_Arrange��$ 
StartValue� 5(      �� $ 
UC_Arrange��$ EndValue���5)      �� $ 
UC_Arrange��$ SilderWidth� 5*      �� $ 
UC_Arrange��$ SliderTickFrequency� 5+      Z�  52      �� 53       6(   �� $ 
UC_Arrange��$ EndRect�55      56      �� 57   
   -   5        $��35��59      $	��0,3q�6   
����d���58      �� $ SL_Bat2��$ Value�$ TwoWay=�5:      5;      5<      