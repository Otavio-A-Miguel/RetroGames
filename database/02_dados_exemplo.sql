-- RetroGames: dados de exemplo (Aula 1, etapa 4)
-- Rode logo depois de 01_criar_tabelas.sql, num banco recém-criado.
-- As chaves são informadas de propósito, para o script ser previsível.
-- Preços, estoques e descrições são fictícios.
USE retrogames;

INSERT INTO tb_condicao_jogo (co_condicao_jogo, ds_condicao_jogo) VALUES
    (1, 'Novo'),
    (2, 'Usado'),
    (3, 'Recondicionado');

INSERT INTO tb_perfil (co_perfil, ds_perfil) VALUES
    (1, 'Administrador'),
    (2, 'Cliente');

INSERT INTO tb_plataforma (co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento) VALUES
    (1, 'Nintendo Entertainment System', 'Nintendo', 1983),
    (2, 'Super Nintendo', 'Nintendo', 1990),
    (3, 'Mega Drive', 'Sega', 1988),
    (4, 'Master System', 'Sega', 1985),
    (5, 'PlayStation', 'Sony', 1994),
    (6, 'Nintendo 64', 'Nintendo', 1996),
    (7, 'Atari 2600', 'Atari', 1977);

INSERT INTO tb_genero (co_seq_genero, no_genero) VALUES
    (1, 'Ação'),
    (2, 'Aventura'),
    (3, 'Corrida'),
    (4, 'Esporte'),
    (5, 'Luta'),
    (6, 'Plataforma'),
    (7, 'Quebra-cabeça'),
    (8, 'RPG'),
    (9, 'Tiro');

INSERT INTO tb_jogo (co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
                     st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo) VALUES
    (1, 'Super Mario Bros.', NULL, 1985, 89.90, 5, TRUE, UTC_TIMESTAMP(), 1, 2),
    (2, 'The Legend of Zelda', 'Clássico de aventura e exploração.', 1986, 129.90, 3, TRUE, UTC_TIMESTAMP(), 1, 2),
    (3, 'Metroid', NULL, 1986, 119.90, 2, TRUE, UTC_TIMESTAMP(), 1, 2),
    (4, 'Mega Man 2', NULL, 1988, 99.90, 4, TRUE, UTC_TIMESTAMP(), 1, 2),
    (5, 'Dr. Mario', NULL, 1990, 59.90, 6, TRUE, UTC_TIMESTAMP(), 1, 2),
    (6, 'Super Mario World', NULL, 1990, 149.90, 4, TRUE, UTC_TIMESTAMP(), 2, 2),
    (7, 'Chrono Trigger', 'RPG clássico com viagem no tempo.', 1995, 399.90, 1, TRUE, UTC_TIMESTAMP(), 2, 3),
    (8, 'Super Metroid', NULL, 1994, 249.90, 2, TRUE, UTC_TIMESTAMP(), 2, 2),
    (9, 'Street Fighter II', NULL, 1992, 119.90, 5, TRUE, UTC_TIMESTAMP(), 2, 2),
    (10, 'F-Zero', NULL, 1990, 129.90, 3, TRUE, UTC_TIMESTAMP(), 2, 3),
    (11, 'Sonic the Hedgehog', NULL, 1991, 79.90, 8, TRUE, UTC_TIMESTAMP(), 3, 2),
    (12, 'Streets of Rage 2', NULL, 1992, 139.90, 2, TRUE, UTC_TIMESTAMP(), 3, 2),
    (13, 'Phantasy Star IV', NULL, 1993, 249.90, 1, TRUE, UTC_TIMESTAMP(), 3, 3),
    (14, 'NBA Jam', NULL, 1994, 69.90, 0, TRUE, UTC_TIMESTAMP(), 3, 2),
    (15, 'Alex Kidd in Miracle World', NULL, 1986, 89.90, 3, TRUE, UTC_TIMESTAMP(), 4, 2),
    (16, 'Phantasy Star', NULL, 1987, 159.90, 1, FALSE, UTC_TIMESTAMP(), 4, 2),
    (17, 'Final Fantasy VII', NULL, 1997, 179.90, 4, TRUE, UTC_TIMESTAMP(), 5, 2),
    (18, 'Gran Turismo', NULL, 1997, 59.90, 6, TRUE, UTC_TIMESTAMP(), 5, 2),
    (19, 'Crash Bandicoot', 'Edição lacrada.', 1996, 299.90, 2, TRUE, UTC_TIMESTAMP(), 5, 1),
    (20, 'Super Mario 64', NULL, 1996, 199.90, 3, TRUE, UTC_TIMESTAMP(), 6, 2),
    (21, 'GoldenEye 007', NULL, 1997, 169.90, 2, TRUE, UTC_TIMESTAMP(), 6, 2);

INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES
    (1, 6), (2, 2), (2, 1), (3, 1), (3, 2), (4, 1), (4, 6), (5, 7),
    (6, 6), (7, 8), (7, 2), (8, 1), (8, 2), (9, 5), (10, 3),
    (11, 6), (12, 1), (12, 5), (13, 8), (14, 4), (15, 6), (15, 2),
    (16, 8), (17, 8), (18, 3), (19, 6), (20, 6), (20, 2), (21, 9), (21, 1);
