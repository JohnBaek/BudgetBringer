<template>
  <v-container>
    <!-- Use a card to contain the table for better styling -->
    <v-card>
      <v-card-title>
        As of 15 Mar. 2024
      </v-card-title>
      <v-card-text>
        <v-simple-table>
          <thead>
          <tr>
            <th class="text-left">Function</th>
            <th class="text-left">Type</th>
            <th class="text-left">2024FY Budget</th>
            <th class="text-left">2023&24FY Approved Amount</th>
            <th class="text-left">2024FY Remaining Budget</th>
            <th class="text-left">CAPEX below CHF500K</th>
          </tr>
          </thead>
          <tbody>
          <tr v-for="item in items" :key="item.function">
            <td>{{ item.function }}</td>
            <td>{{ item.type }}</td>
            <td>{{ formatNumber(item.budget2024) }}</td>
            <td>{{ formatNumber(item.approved) }}</td>
            <td>{{ formatNumber(item.remaining) }}</td>
            <td>{{ formatNumber(item.capexBelow) }}</td>
          </tr>
          </tbody>
          <tfoot>
          <tr>
            <td>TOTAL KOREA</td>
            <td>CAPEX</td>
            <td>{{ formatNumber(total.budget2024) }}</td>
            <td>{{ formatNumber(total.approved) }}</td>
            <td>{{ formatNumber(total.remaining) }}</td>
            <td></td> <!-- Empty cell for the last column footer -->
          </tr>
          </tfoot>
        </v-simple-table>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';

export default defineComponent({
  name: 'BudgetTable',
  setup() {
    const items = ref([
      // Add your rows here
      {
        function: '영업본부 상무',
        type: 'CAPEX',
        budget2024: 1357368,
        approved: 873660,
        remaining: -483708,
        capexBelow: -483708, // Replace with actual data
      },
      // ...other items
    ]);

    const total = ref({
      budget2024: 8117442,
      approved: 5546357,
      remaining: -2571085,
      capexBelow: -2571085, // Replace with actual data
    });

    function formatNumber(value: number): string {
      return new Intl.NumberFormat('en-US', { maximumFractionDigits: 0 }).format(value);
    }

    return { items, total, formatNumber };
  }
});
</script>

<style scoped>
/* Add styles here if needed */
</style>
