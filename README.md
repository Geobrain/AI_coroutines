
Пример:
запускается сцена AIstart.

- при старте Mob постоянно движется влево, потом вправо.
- при нажатии на кнопку Q будет писать "qqq".
- при нажатии на кнопку W будет писать "www" в течении 10 секунд - после этого поведение прекратит работу и снова моб будет двигаться влево-вправо

количество поведений 10 у одной сущности, но можно выставить 127 и более в компоненте и процессоре.
поведения находятся в ModelMobAI 
вверху находятся поведения с приоритетом 0  - чем ниже поведение расположено в скрипте - тем выше у него приоритет
EnableBehaviour - метод включения поведения у entity. через него можно передать другое entAnother в поведение - например, чтобы entity напал на entAnother

ps.
общая теория по аи Pixeye
