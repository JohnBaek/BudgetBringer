<script setup lang="ts">
</script>

<template>
  <v-app>
    <v-main>
      <v-container fluid>
        <v-row>
          <!-- 리스트 영역 -->
          <v-col cols="12" md="2">
            <div class="scrollable-list bordered-list elevation-3">
              <v-list  >
                <v-list-item
                  v-for="n in 100"
                  :key="n"
                  :title="'Item ' + n"
                  subtitle="Lorem ipsum dolor sit amet consectetur adipisicing elit"
                ></v-list-item>
              </v-list>
            </div>
          </v-col>

          <v-col cols="12" md="10">
            <div class="elevation-3">
              <!-- 메인 컨텐츠 -->
              <!-- 메인 컨텐츠 -->
              ㅇㅇㅇ
              ㅇㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇㅇ
              <br>ㅇ
              <br>ㅇ


              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇㅇ

              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇ
              <br>ㅇㅇ

            </div>
          </v-col>

        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<style>
.bordered-list {
  border: 1px solid rgba(0, 0, 0, 0.12); /* Vuetify의 테마에 맞는 색상 */
  height: 100vh; /* 뷰포트의 높이에 맞춤 */
  max-height: 100vh; /* 최대 높이를 뷰포트의 높이로 제한 */
  overflow-y: auto; /* 내용이 넘칠 경우 스크롤바 표시 */
}
.scrollable-content {
  height: 100vh; /* 뷰포트의 높이에 맞춤 */
  max-height: 100vh; /* 최대 높이를 뷰포트의 높이로 제한 */
  overflow-y: auto; /* 내용이 넘칠 경우 스크롤바 표시 */
}
</style>

<!--<template>-->
<!--  <div class="d-flex flex-row">-->
<!--    &lt;!&ndash; 리스트 영역 &ndash;&gt;-->
<!--    <div class="flex-list" style="border: 1px solid gray">-->
<!--      <v-list lines="one">-->
<!--      <v-list-item-->
<!--        v-for="n in 100"-->
<!--        :key="n"-->
<!--        :title="'Item ' + n"-->
<!--        subtitle="Lorem ipsum dolor sit amet consectetur adipisicing elit"-->
<!--      ></v-list-item>-->
<!--    </v-list>-->
<!--    </div>-->

<!--    &lt;!&ndash; 콘텐츠 영역 &ndash;&gt;-->
<!--    <div class="flex-content" style="border: 1px solid red">-->
<!--      dddddddd-->
<!--    </div>-->
<!--  </div>-->
<!--  <v-container>-->

<!--&lt;!&ndash;    &lt;!&ndash; 전체 레이아웃을 위한 행 &ndash;&gt;&ndash;&gt;-->
<!--&lt;!&ndash;    <v-row no-gutters>&ndash;&gt;-->
<!--&lt;!&ndash;      <v-col>&ndash;&gt;-->
<!--&lt;!&ndash;        <div  style="border:1px solid red">&ndash;&gt;-->
<!--&lt;!&ndash;          <v-list lines="one">&ndash;&gt;-->
<!--&lt;!&ndash;            <v-list-item&ndash;&gt;-->
<!--&lt;!&ndash;              v-for="n in 100"&ndash;&gt;-->
<!--&lt;!&ndash;              :key="n"&ndash;&gt;-->
<!--&lt;!&ndash;              :title="'Item ' + n"&ndash;&gt;-->
<!--&lt;!&ndash;              subtitle="Lorem ipsum dolor sit amet consectetur adipisicing elit"&ndash;&gt;-->
<!--&lt;!&ndash;              :prepend-avatar="'https://randomuser.me/api/portraits/women/8.jpg'"&ndash;&gt;-->
<!--&lt;!&ndash;            ></v-list-item>&ndash;&gt;-->
<!--&lt;!&ndash;          </v-list>&ndash;&gt;-->
<!--&lt;!&ndash;        </div>&ndash;&gt;-->
<!--&lt;!&ndash;      </v-col>&ndash;&gt;-->
<!--&lt;!&ndash;      <v-col cols="9">&ndash;&gt;-->
<!--&lt;!&ndash;        <div  style="border:1px solid red">&ndash;&gt;-->
<!--&lt;!&ndash;          .v-col-6&ndash;&gt;-->
<!--&lt;!&ndash;        </div>&ndash;&gt;-->
<!--&lt;!&ndash;      </v-col>&ndash;&gt;-->
<!--&lt;!&ndash;    </v-row>&ndash;&gt;-->
<!--  </v-container>-->
<!--</template>-->



<!--<style scoped>-->
<!--.d-flex {-->
<!--  display: flex;-->
<!--}-->

<!--.flex-row {-->
<!--  flex-direction: row;-->
<!--}-->

<!--.flex-list {-->
<!--  flex: 3; /* 리스트 영역이 차지할 비율 */-->
<!--  max-width: 300px; /* 최대 너비 */-->
<!--  /* 추가 스타일링 */-->
<!--}-->

<!--.flex-content {-->
<!--  flex: 7; /* 콘텐츠 영역이 차지할 비율 */-->
<!--  /* 추가 스타일링 */-->
<!--}-->
<!--</style>-->
