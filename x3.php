<?php
// Define a chave de criptografia gerada no servidor
$valor = "0F 0D 00 00 00 99 88 06 01 07 44 00 00 83 49 56 67 00 43 22 02 65 91 00 04 54 60 03"; // adcione sua chave privada aqui!

// Codifica a chave em base64 para torná-la "oculta" e não legível facilmente
$chaveCodificada = base64_encode($valor);

// Configura o cabeçalho para retornar a chave codificada (sem exibir conteúdo no corpo)
header('X-Custom-Key: ' . $chaveCodificada);

// Configura o código de status HTTP para 204 (sem conteúdo no corpo)
http_response_code(204);

// Finaliza o script para garantir que não seja exibido nada no corpo da resposta
exit;
?>