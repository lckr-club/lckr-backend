INSERT INTO users (nome, email, senha, organization_cnpj) VALUES
('Secretaria', 'secretaria@lckrclub.com.br', '123456', '46729600000196');

INSERT INTO coaches (cpf, nome, email, senha, telefone, data_admissao, especialidade, status, salario_atual, organization_cnpj) VALUES
('47291368805', 'Ana Beatriz Ramalho', 'ana@lckrclub.com.br', '123456', '48999990001', '2025-02-01', 'CrossFit', 'ativo', 62.50, '46729600000196'),
('31822647011', 'Carlos Eduardo Nunes', 'carlos@lckrclub.com.br', '123456', '48999990002', '2025-03-10', 'Hyrox', 'ativo', 78.00, '46729600000196'),
('58301926477', 'Juliana Castro Meireles', 'juliana@lckrclub.com.br', '123456', '48999990003', '2025-01-20', 'CrossFit', 'inativo', 58.00, '46729600000196');

INSERT INTO lessons (data_aula, hora_inicio, hora_fim, capacidade, status, descricao, cpf_coach) VALUES
('2026-08-24', '06:00', '07:00', 20, 'realizada', 'CrossFit', '47291368805'),
('2026-08-24', '07:00', '08:00', 20, 'realizada', 'CrossFit', '47291368805'),
('2026-08-25', '18:00', '19:00', 20, 'realizada', 'Hyrox', '31822647011');
