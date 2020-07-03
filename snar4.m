[testName,testPath]=uigetfile( ...       %вызов диалогового окна для выбора файлов
{  '*.txt','TXT-files (*.txt)'; ...            %изначально показывает файлы в папке с расширением txt
   '*.*',  'All Files (*.*)'}, ...               %можно выбрать показывать все файлы
   'Выберите данные, содержащие точки', ...   %Заголовок диалогового окна
   'MultiSelect', 'off');                         %Выключение режима мультиселекта (больше одного файла)
  %Вызываем функцию, которая меняет в файле запятые на точки (так как матлаб считывает дроби с точкой)
if (testName~=0)
    copyfile([testPath,testName], 'testWithPoint.txt'); % Копируем файл в вспомогательный txt
    comma2point_overwrite( 'testWithPoint.txt' );
    N = dlmread('testWithPoint.txt',"\t",[1,0,1,0]);
    X = dlmread('testWithPoint.txt',"\t",[2,0,N + 1,0]);
    Y = dlmread('testWithPoint.txt',"\t",[2,1,N + 1,1]);
    Z = dlmread('testWithPoint.txt',"\t",[2,2,N + 1,2]);
end
[ansName,ansPath]=uigetfile( ...       %вызов диалогового окна для выбора файлов
{  '*.txt','TXT-files (*.txt)'; ...            %изначально показывает файлы в папке с расширением txt
   '*.*',  'All Files (*.*)'}, ...               %можно выбрать показывать все файлы
   'Выберите данные, содержащие ответ', ...   %Заголовок диалогового окна
   'MultiSelect', 'off');                         %Выключение режима мультиселекта (больше одного файла)
if (ansName~=0)
    copyfile([ansPath,ansName], 'ansWithPoint.txt'); % Копируем файл в вспомогательный txt
    comma2point_overwrite( 'ansWithPoint.txt' );
    A = dlmread('ansWithPoint.txt'," ",[0,0,0,0]);
    B = dlmread('ansWithPoint.txt'," ",[0,1,0,1]);
    C = dlmread('ansWithPoint.txt'," ",[0,2,0,2]);
    D = dlmread('ansWithPoint.txt'," ",[0,3,0,3]);
end
p=abs(A.*X+B.*Y+C.*Z+D)./sqrt(A^2+B^2+C^2);
[x,y] = meshgrid(-100:1:100,-100:1:100);
z = (-A .* x - B .* y - D)./C;
figure(1) % Открывает новое окно с фигурой
surf(x,y,z, 'FaceAlpha', 0.5, 'LineStyle', 'none')
hold on; % холд ставится после вывода перовго графика
plot3(X,Y,Z,'ro')
xlabel('Координата X');
ylabel('Координата Y');
zlabel('Координата Z');
titl=sprintf('График плоскости с коэффициентами A = %.6f, B = %.6f, C = %.6f, D = %.6f,',A,B,C,D);
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