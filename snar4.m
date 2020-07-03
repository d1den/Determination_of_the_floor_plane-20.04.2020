[testName,testPath]=uigetfile( ...       %����� ����������� ���� ��� ������ ������
{  '*.txt','TXT-files (*.txt)'; ...            %���������� ���������� ����� � ����� � ����������� txt
   '*.*',  'All Files (*.*)'}, ...               %����� ������� ���������� ��� �����
   '�������� ������, ���������� �����', ...   %��������� ����������� ����
   'MultiSelect', 'off');                         %���������� ������ ������������� (������ ������ �����)
  %�������� �������, ������� ������ � ����� ������� �� ����� (��� ��� ������ ��������� ����� � ������)
if (testName~=0)
    copyfile([testPath,testName], 'testWithPoint.txt'); % �������� ���� � ��������������� txt
    comma2point_overwrite( 'testWithPoint.txt' );
    N = dlmread('testWithPoint.txt',"\t",[1,0,1,0]);
    X = dlmread('testWithPoint.txt',"\t",[2,0,N + 1,0]);
    Y = dlmread('testWithPoint.txt',"\t",[2,1,N + 1,1]);
    Z = dlmread('testWithPoint.txt',"\t",[2,2,N + 1,2]);
end
[ansName,ansPath]=uigetfile( ...       %����� ����������� ���� ��� ������ ������
{  '*.txt','TXT-files (*.txt)'; ...            %���������� ���������� ����� � ����� � ����������� txt
   '*.*',  'All Files (*.*)'}, ...               %����� ������� ���������� ��� �����
   '�������� ������, ���������� �����', ...   %��������� ����������� ����
   'MultiSelect', 'off');                         %���������� ������ ������������� (������ ������ �����)
if (ansName~=0)
    copyfile([ansPath,ansName], 'ansWithPoint.txt'); % �������� ���� � ��������������� txt
    comma2point_overwrite( 'ansWithPoint.txt' );
    A = dlmread('ansWithPoint.txt'," ",[0,0,0,0]);
    B = dlmread('ansWithPoint.txt'," ",[0,1,0,1]);
    C = dlmread('ansWithPoint.txt'," ",[0,2,0,2]);
    D = dlmread('ansWithPoint.txt'," ",[0,3,0,3]);
end
p=abs(A.*X+B.*Y+C.*Z+D)./sqrt(A^2+B^2+C^2);
[x,y] = meshgrid(-100:1:100,-100:1:100);
z = (-A .* x - B .* y - D)./C;
figure(1) % ��������� ����� ���� � �������
surf(x,y,z, 'FaceAlpha', 0.5, 'LineStyle', 'none')
hold on; % ���� �������� ����� ������ ������� �������
plot3(X,Y,Z,'ro')
xlabel('���������� X');
ylabel('���������� Y');
zlabel('���������� Z');
titl=sprintf('������ ��������� � �������������� A = %.6f, B = %.6f, C = %.6f, D = %.6f,',A,B,C,D);
title(titl,'FontSize',14);
%axis([-100 100 -100 100 -100 100])

function    comma2point_overwrite( filespec )
    % replaces all occurences of comma (",") with point (".") in a text-file.
    % Note that the file is overwritten, which is the price for high speed.
        file    = memmapfile( filespec, 'writable', true );
        comma   = uint8(',');
        point   = uint8('.');
        file.Data( transpose( file.Data==comma) ) = point;
end