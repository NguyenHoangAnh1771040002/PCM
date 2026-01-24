<template>
  <v-container class="text-center py-16">
    <v-icon icon="mdi-alert-circle-outline" size="100" color="warning" />
    <h1 class="text-h3 mt-4">404</h1>
    <h2 class="text-h5 text-grey mb-4">Không tìm thấy trang</h2>
    <p class="text-body-1 text-grey mb-6">
      Trang bạn đang tìm kiếm không tồn tại hoặc đã bị di chuyển.
    </p>
    <v-btn color="primary" size="large" :to="{ name: 'Home' }">
      <v-icon left>mdi-home</v-icon>
      Về trang chủ
    </v-btn>
  </v-container>
</template>
