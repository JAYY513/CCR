   M S B A M L   `   `   `  ����B  >CCRMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:CCRMainCCRMain  5   	       CCRMain.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   S OMaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null;wpf1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   !localclr-namespace:CCRMain  5   	     � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)    � 	Resources 5
       �� 
TargetType �� Property �� Value��%Q   '��       �� 5   
   " ��6   .�5      ��  � ��$ 3q�6'   5      5      .��5      � .+�5   
   � $��Center=�6   $	��210��6,   $��Center=�68    � ColumnDefinitions 5      �� 5      $
��AUTO �6#   62   �� 5      6%    � RowDefinitions 5      �� 5      6"   �� 5      6"   �� 5      6"   �� 5      6"   �� 5      6"   �� 5      6"   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Account=�6   $��Center=�6*   6G   �� 5      -   5        $
��user��5   &   $��1��6   .��62   1234567891234566C     5       $
 Lock=�6   $��1��6'   $��Center=�64   6Q   �� 5!      -   5        $	��pwd��5!   3   $��1��6   $��1��6&   .��6>   1234566F     5#      $	 Abc=�6   $��2��6&   $��Center=�63   6P   �� 5$      -   5        $��employeeAccount��5$   3   $��1��6   $��2��6&   .��6J   
123456786T     5&      $ 
CodeString=�6   $��3��6-   $��Center=�6:   6W   �� 5'      -   5        $
��code��5'   3   $��1��6   $��3��6&   6A   �� 5(      $��4��6   $��2��6&   $��30��6:   $��3q�6F   $��Center=�6Q    �� Orientation$ 
Horizontal=�6n   .+�5)      �� -   5        $��codeImg��5)   #   $��69��6   64   �� 5*      -   5        .��5*   2   获取验证码69   5+      �� 5,      -   5        $��5��5,      $��2��6"   $��Button��67   $��Center=�6H   $	��Top=�6e   $��75��6}   6�   5-      �� 5.   
   -   5        $��Button��5.      $
��Left=�6"   $��
589,94,0,0q�6=   $	��Top=�6Q   $��75��6i   6t   5/      50      