-- RetroGames: testes de integridade (Aula 1, etapa 5)
-- Execute UMA instrução por vez e preencha o resultado obtido.
USE retrogames;

-- Teste 1: jogo com co_plataforma inexistente
-- Esperado: Erro 1452 (chave estrangeira)
-- Obtido: Erro 1452
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('Teste FK', 1990, 10.00, 1, UTC_TIMESTAMP(), 999, 1);

-- Teste 2: plataforma com nome já cadastrado
-- Esperado: Erro 1062 (UNIQUE)
-- Obtido: Erro 1062
INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento)
VALUES ('Mega Drive', 'Sega', 1988);

-- Teste 3: excluir plataforma que possui jogos
-- Esperado: Erro 1451 (chave estrangeira)
-- Obtido: Erro 1451
DELETE FROM tb_plataforma WHERE co_seq_plataforma = 1;

-- Teste 4: jogo com no_jogo nulo
-- Esperado: Erro 1048 (NOT NULL)
-- Obtido: Erro 1048
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES (NULL, 1990, 10.00, 1, UTC_TIMESTAMP(), 1, 1);

-- Teste 5: jogo com vl_preco igual a 0
-- Esperado: aceito (a regra RN06 fica no Service, não no banco)
-- Obtido: Aceito
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('Teste preco zero', 1990, 0, 1, UTC_TIMESTAMP(), 1, 1);

-- Limpeza: remove o jogo inserido pelo teste 5
DELETE FROM tb_jogo WHERE co_seq_jogo > 21;
