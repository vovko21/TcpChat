   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient  5   	       Client.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Client     5   	   b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5           MaterialDesignBody#���   5	   	   $��Regular"�5
   	   $��13(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper#���  5   	     MaterialDesignFont#���  5   	    � Title$ 
MainWindow��5   	   $	��450��6   $	��800��6)   .��5      �  � ColumnDefinitions 5   
   �� 5      $��80��6   $
��Auto �6-   �� 5      $	��150��6   �� 5      $
��Auto �6   �� 5      $	��300��6   .+�5   
   9� -   5         9�Name$ ExportExpander��5   #   $��0��6    9� ExpandDirection$ Right=�6;   .��5      �� .+�5      A� $��50��6   $��50��6%   ��5      �� 5       �� ImageSource 6%   �� *ClientImageUI+ �� Mode$ OneWay=� �� UpdateSourceTrigger$ PropertyChanged=�6   5      �� 5         MaterialDesignLabel#��[ 6   $��Center=�5!      $��50��5#      $��25��6'   ��5"      �� *ClientNameUI+$ OneWay=�5#   5   �� 5$        MaterialDesignDarkSeparator#��[ 6   6S   �� 5%      $��Settings��6     MaterialDesignFlatButton#��[ 6,   ��6^   �� *ShowSettings_cm+6�   5&      5'      �� 5(   
   $��1��6   	 �� ShowsPreview	 . $��3��6;   $��Right=�5)      �� 5*   
   $��1��6   .+�5+      �� 
 �� Orientation$
 
Horizontal=�6   $
��Auto��62   .+�5,      �� )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Search��6   $��5q�6B   $	��100��6M   ��6Y   �� *GlobalSearch_cm+6|   �� 5-       " MaterialDesignFlatDarkBgButton#��[ 6   $��40��5.      $��5q�6$   $��Right=�6/   $��S��5/      ��6%   �� *	Search_cm+6E   50      �� 51      -   5        $��1��51        MaterialDesignListBox#��[ 6&   $
��Auto��52      $
��Auto��6#   $
��Left=�61   ��53      �� *
FriendList+ �� SelectedItem 54      �� *SelectedClient+��56      �� 57      .R�58      �� $��5q�6%   $
 
Horizontal=�60   .+�59      A� $��40��6&   $��40��61   ��5:   "   �� 5;   &    61   �� *	ImagePath+$ OneWay=�$ PropertyChanged=�6�   5=      �� 5>      $��16(�6(   $��Center=�6Q   $��Center=�6n   ��66   ��  �� Path$
 Name�6n   5?      5@      5B      5C      �� 5D   
   $��3��6   .+�5E      �� #��[ 5F      $	��356��5G      $	��380��6"   $��Right=�6/   $��1q�5H      
���   �$	��Top=�5I      ��5E      �� *ChatMessages+5I   0   �� 5J      $��Bottom=�6   $
 
Horizontal=�64   $��Right=�6M   .+�5K      �� $	��150��6   $��30��6?   ��6&   �� *	Message+6M   �� 5L      ��6   �� *SendMessage_cm+.��6<   Send6B   5M      5N      5O      5P      